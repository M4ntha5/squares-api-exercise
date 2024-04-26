using FluentValidation;
using Validation.Exceptions;

namespace Validation
{
    public class ValidatorsAdapter
    {
        private readonly IEnumerable<IValidator> _validators;

        public ValidatorsAdapter(IEnumerable<IValidator> validators)
        {
            _validators = validators;
        }

        public Task ValidateAndThrowAsync<T>(T dto) where T : class
        {
            var validator = GetValidator<T>();

            return validator.ValidateAndThrowAsync(dto);
        }

        public Task ValidateAndThrowAsync<T>(T dto, params string[] ruleSets) where T : class
        {
            var validator = GetValidator<T>();

            return validator.ValidateAsync(dto, options => options.IncludeRuleSets(ruleSets).ThrowOnFailures());
        }

        private IValidator<TEntityDto> GetValidator<TEntityDto>() where TEntityDto : class
        {
            if (_validators.FirstOrDefault(v => v is IValidator<TEntityDto>) is not IValidator<TEntityDto>
                validator)
                throw ValidatorsAdapterException.ClassNotRegisteredException(typeof(IValidator<TEntityDto>));

            return validator;
        }
    }
}