using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TK_UR_BOOK.Infrastructure.Persistence.Converters
{
    public class StronglyTypedIdConverter<TId , TValue> : ValueConverter<TId, TValue>
        where TId : class
    {
        public StronglyTypedIdConverter()
            : base(
                  id => (TValue)typeof(TId).GetProperty("Value")!.GetValue(id)!,
                  value =>(TId)Activator.CreateInstance(typeof(TId), value)!)
        {
        }
    }
}
