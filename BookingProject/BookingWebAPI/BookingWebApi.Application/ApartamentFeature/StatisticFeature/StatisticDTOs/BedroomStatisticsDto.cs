using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.ApartamentFeature.StatisticFeature.StatisticDTOs;

public record BedroomStatisticsDto(
    int Bedrooms,
    double AverageArea
);

