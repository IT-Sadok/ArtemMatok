using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Apartament.Statistics.StatisticDTOs;

public record BedroomStatisticsDto(
    int Bedrooms,
    double AverageArea
);

