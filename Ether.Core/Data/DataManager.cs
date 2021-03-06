﻿using Ether.Core.Interfaces;
using Ether.Core.Models.DTO;
using System.Collections.Generic;

namespace Ether.Core.Data
{
    public class DataManager<TData> : IAll<TData>
        where TData: BaseDto
    {
        private readonly IRepository _repository;
        private IEnumerable<TData> _cache;

        public DataManager(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<TData> Value
        {
            get
            {
                if(_cache == null)
                    _cache = _repository.GetAll<TData>();

                return _cache;
            }
        }
    }
}
