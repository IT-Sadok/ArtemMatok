using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    public class SagaManager
    {
        private readonly List<(Func<Task<Result<object>>> Step, List<Func<Task>> Compensations)> _stepsWithCompensations = new();

        public SagaManager AddStep(Func<Task<Result<object>>> step, List<Func<Task>> compensations)
        {
            _stepsWithCompensations.Add((step, compensations?.ToList() ?? new List<Func<Task>>()));
            return this;
        }

        public async Task<Result<object>> ExecuteAsync()
        {
            var executedCompensations = new Stack<Func<Task>>();

            foreach (var (step, compensations) in _stepsWithCompensations)
            {
                var result = await step();
                if (!result.IsSuccess)
                {
                    AddCompensations(compensations, executedCompensations);

                    await CompensateAsync(executedCompensations);
                    return Result<object>.Failure(result.ErrorMessage);
                }

                AddCompensations(compensations, executedCompensations);
            }

            return Result<object>.Success(null);
        }

        private void AddCompensations(IEnumerable<Func<Task>> compensations, Stack<Func<Task>> executedCompensations)
        {
            if (compensations != null && compensations.Any())
            {
                foreach (var compensation in compensations)
                {
                    executedCompensations.Push(compensation);
                }
            }
        }

        private async Task CompensateAsync(Stack<Func<Task>> executedCompensations)
        {
            while (executedCompensations.Count > 0)
            {
                var compensation = executedCompensations.Pop();
                await compensation();
            }
        }
    }
}
