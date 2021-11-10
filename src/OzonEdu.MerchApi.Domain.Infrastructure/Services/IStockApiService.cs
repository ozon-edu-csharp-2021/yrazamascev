﻿using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.HttpModels;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Services
{
    public interface IStockApiService
    {
        Task<List<StockItemResponse>> GetAll(CancellationToken cancellationToken);

        Task<bool> Reserve(List<SkuPack> skuPacks, CancellationToken cancellationToken);
    }
}