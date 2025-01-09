using DistributedLocking;
using FluentAssertions;
using Moq;
using Payment.Application.Interfaces.PaymentInterface;
using Payment.Application.Services.PaymentService;
using Payment.Infrastructure.Interfaces.OutboxInterface;
using Payment.Infrastructure.Interfaces.PaymentInterface;
using Xunit;

namespace Payment.Tests.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly IPaymentService _paymentService;
        private readonly Mock<IDistributedLockService> _distributedLockServiceMock;
        private readonly Mock<IOutboxRepository> _outboxrepository;


        public PaymentServiceTests()
        {
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _paymentService = new PaymentService(_paymentRepositoryMock.Object, _distributedLockServiceMock.Object, _outboxrepository.Object);
        }

        [Fact]
        public async Task CreateBalanceAsync_WithValidUserId_ShouldReturnTrue()
        {
            var userId = "valid-user-id";
            _paymentRepositoryMock
                .Setup(repo => repo.CreateBalanceAsync(userId))
                .ReturnsAsync(true);

            var result = await _paymentService.CreateBalanceAsync(userId);

            result.Should().BeTrue();
            _paymentRepositoryMock.Verify(repo => repo.CreateBalanceAsync(userId), Times.Once);
        }

        [Fact]
        public async Task CreateBalanceAsync_WithEmptyUserId_ShouldReturnFalse()
        {
            var userId = "";

            var result = await _paymentService.CreateBalanceAsync(userId);

            result.Should().BeFalse();
            _paymentRepositoryMock.Verify(repo => repo.CreateBalanceAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CreateBalanceAsync_WithNullUserId_ShouldReturnFalse()
        {
            string userId = null;

            var result = await _paymentService.CreateBalanceAsync(userId);

            result.Should().BeFalse();
            _paymentRepositoryMock.Verify(repo => repo.CreateBalanceAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
