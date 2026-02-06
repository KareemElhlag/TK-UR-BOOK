using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TK_UR_BOOK.Infrastructure.Persistence.Converters
{
    public class StronglyTypedIdConverter<TId> : ValueConverter<TId, Guid>
        where TId : class
    {
        public StronglyTypedIdConverter()
            : base(
                  id => (Guid)typeof(TId).GetProperty("Value")!.GetValue(id)!,
                  value =>(TId)Activator.CreateInstance(typeof(TId), value)!)
        {
        }
    }
}
