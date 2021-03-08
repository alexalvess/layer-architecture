using FluentValidation;
using Layer.Architecture.Domain.Entities;
using System.Collections.Generic;

namespace Layer.Architecture.Domain.Interfaces
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        TOutputModel Add<TInputModel, TOutputModel, TValidator>(TInputModel inputModel)
            where TValidator : AbstractValidator<TEntity>
            where TInputModel : class
            where TOutputModel : class;

        void Delete(int id);

        IEnumerable<TOutputModel> Get<TOutputModel>() where TOutputModel : class;

        TOutputModel GetById<TOutputModel>(int id) where TOutputModel : class;

        TOutputModel Update<TInputModel, TOutputModel, TValidator>(TInputModel inputModel)
            where TValidator : AbstractValidator<TEntity>
            where TInputModel : class
            where TOutputModel : class;
    }
}
