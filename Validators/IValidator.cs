namespace Lab1
{
    public interface IValidator<E>
    {
        /**
     * @param entity the entity to be validated
     * @throws ValidationException if the entity doesnt meet the specified characteristics
     */
        void Validate(E entity);
    }
}