using Prometheus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Apartament
{
    public static class ApartamentCustomMetrics
    {
        public static readonly Counter SuccessfulApartamentCreations = Metrics.CreateCounter(
            "successful_apartament_creations_total",
            "Total number of successfully created apartments.");

    }
}
