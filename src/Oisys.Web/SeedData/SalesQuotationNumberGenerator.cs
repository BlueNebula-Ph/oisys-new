namespace OisysNew.SeedData
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.ValueGeneration;

    /// <summary>
    /// This is used for auto generating codes.
    /// Should not be used when migrated to sql server.
    /// </summary>
    public class SalesQuotationNumberGenerator : ValueGenerator<int>
    {
        private const int InitialCode = 100001;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesQuotationNumberGenerator"/> class.
        /// </summary>
        public SalesQuotationNumberGenerator()
        {
        }

        /// <inheritdoc />
        public override bool GeneratesTemporaryValues => false;

        /// <inheritdoc/>
        public override int Next(EntityEntry entry)
        {
            return ((OisysDbContext)entry.Context).SalesQuotes.LastOrDefault()?.QuoteNumber + 1 ?? InitialCode;
        }
    }
}
