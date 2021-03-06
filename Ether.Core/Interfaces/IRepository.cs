﻿using Ether.Core.Models.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ether.Core.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<T>> GetAllAsync<T>() where T: BaseDto;

        IEnumerable<T> GetAll<T>() where T : BaseDto;

        Task<IEnumerable> GetAllByTypeAsync(Type itemType);

        Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseDto;

        IEnumerable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : BaseDto;

        Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseDto;

        Task<TProjection> GetFieldValueAsync<TType, TProjection>(Expression<Func<TType, bool>> predicate, Expression<Func<TType, TProjection>> projection) where TType : BaseDto;

        TProjection GetFieldValue<TType, TProjection>(Expression<Func<TType, bool>> predicate, Expression<Func<TType, TProjection>> projection) where TType : BaseDto;

        Task UpdateFieldValue<T, TField>(T obj, Expression<Func<T, TField>> field, TField value) where T : BaseDto;

        Task<object> GetSingleAsync(Guid id, Type itemType);

        Task<T> GetSingleAsync<T>(Guid id) where T : BaseDto;

        Task<bool> CreateAsync<T>(T item) where T : BaseDto;

        Task<bool> CreateOrUpdateAsync<T>(T item) where T : BaseDto;

        Task<bool> CreateOrUpdateAsync<T>(T item, Expression<Func<T, bool>> criteria) where T : BaseDto;

        Task<bool> DeleteAsync<T>(Guid id) where T : BaseDto;

        long Delete<T>(Expression<Func<T, bool>> predicate) where T : BaseDto;
    }
}
