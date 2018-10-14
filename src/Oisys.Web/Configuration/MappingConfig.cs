using AutoMapper;
using OisysNew.DTO.Category;
using OisysNew.DTO.Province;
using OisysNew.Models;
using System.Linq;

namespace OisysNew
{
    /// <summary>
    /// <see cref="MappingConfig"/> class Mapping configuration.
    /// </summary>
    public class MappingConfig : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingConfig"/> class.
        /// </summary>
        public MappingConfig()
        {
            // Category
            this.CreateMap<Category, CategorySummary>();
            this.CreateMap<Category, CategoryLookup>();
            this.CreateMap<SaveCategoryRequest, Category>();

            // City
            this.CreateMap<City, CitySummary>()
                .ForMember(d => d.ProvinceName, s => s.MapFrom(o => o.Province.Name));
            this.CreateMap<SaveCityRequest, City>();

            // Province
            this.CreateMap<Province, ProvinceSummary>();
            this.CreateMap<Province, ProvinceLookup>()
                .ForMember(d => d.Cities, s => s.MapFrom(o => o.Cities.OrderBy(c => c.Name)));
            this.CreateMap<SaveProvinceRequest, Province>();

            // // Adjustment
            // this.CreateMap<Adjustment, ItemAdjustmentSummary>()
            //     .ForMember(d => d.Item, s => s.MapFrom(o => o.Item.Name))
            //     .ForMember(d => d.AdjustmentType, s => s.MapFrom(o => $"{o.AdjustmentType} - {o.QuantityType}"));

            // // Cash Voucher
            // this.CreateMap<CashVoucher, CashVoucherSummary>();
            // this.CreateMap<SaveCashVoucherRequest, CashVoucher>();

            // // Credit Memo
            // this.CreateMap<CreditMemo, CreditMemoSummary>();
            // this.CreateMap<SaveCreditMemoRequest, CreditMemo>();
            // this.CreateMap<CreditMemoDetail, CreditMemoDetailSummary>()
            //     .ForMember(d => d.OrderCode, s => s.MapFrom(o => o.OrderDetail.Order.Code))
            //     .ForMember(d => d.ItemCode, s => s.MapFrom(o => o.OrderDetail.Item.Code))
            //     .ForMember(d => d.Item, s => s.MapFrom(o => o.OrderDetail.Item.Name))
            //     .ForMember(d => d.Price, s => s.MapFrom(o => o.OrderDetail.Price))
            //     .ForMember(d => d.ShouldAddBackToInventory, s => s.MapFrom(o => o.ReturnedToInventory));
            // this.CreateMap<SaveCreditMemoDetailRequest, CreditMemoDetail>()
            //     .ForMember(d => d.ReturnedToInventory, s => s.MapFrom(o => o.ShouldAddBackToInventory));

            // // TODO: Change LastUpdatedBy value
            // // Customer
            // this.CreateMap<Customer, CustomerLookup>();
            // this.CreateMap<Customer, CustomerWithOrdersLookup>();

            // this.CreateMap<Customer, CustomerSummary>()
            //     .ForMember(d => d.CityName, s => s.MapFrom(o => o.City.Name))
            //     .ForMember(d => d.ProvinceName, s => s.MapFrom(o => o.Province.Name))
            //     .ForMember(d => d.PriceList, s => s.MapFrom(o => o.PriceList.Code))
            //     .ForMember(d => d.PriceListId, s => s.MapFrom(o => o.ProvinceId.ToString()));

            // this.CreateMap<SaveCustomerRequest, Customer>();

            // // Customer Transaction
            // // TODO: Create method to compute running balance
            // this.CreateMap<SaveCustomerTrxRequest, CustomerTransaction>();

            // this.CreateMap<CustomerTransaction, CustomerTransactionSummary>()
            //     .ForMember(d => d.Customer, s => s.MapFrom(o => o.Customer.Name));

            // // Delivery
            // this.CreateMap<Delivery, DeliverySummary>()
            //     .ForMember(d => d.ProvinceName, s => s.MapFrom(o => o.Province.Name))
            //     .ForMember(d => d.CityName, s => s.MapFrom(o => o.City.Name));
            // this.CreateMap<SaveDeliveryRequest, Delivery>();

            // // Delivery Detail
            // this.CreateMap<DeliveryDetail, DeliveryDetailSummary>()
            //     .ForMember(d => d.CustomerName, s => s.MapFrom(o => o.OrderDetail.Order.Customer.Name))
            //     .ForMember(d => d.CategoryName, s => s.MapFrom(o => o.OrderDetail.Item.Category.Name))
            //     .ForMember(d => d.ItemCode, s => s.MapFrom(o => o.OrderDetail.Item.Code))
            //     .ForMember(d => d.ItemName, s => s.MapFrom(o => o.OrderDetail.Item.Name))
            //     .ForMember(d => d.OrderNumber, s => s.MapFrom(o => o.OrderDetail.Order.Code.ToString()))
            //     .ForMember(d => d.ItemCodeName, s => s.MapFrom(o => $"{o.OrderDetail.Item.Code} - {o.OrderDetail.Item.Name}"))
            //     .ForMember(d => d.Unit, s => s.MapFrom(o => o.OrderDetail.Item.Unit));
            // this.CreateMap<SaveDeliveryDetailRequest, DeliveryDetail>();

            // // Invoice
            // this.CreateMap<Invoice, InvoiceSummary>()
            //     .ForMember(d => d.Customer, s => s.MapFrom(o => o.Customer.Name));
            // this.CreateMap<SaveInvoiceRequest, Invoice>();

            // // Invoice Detail
            // this.CreateMap<SaveInvoiceDetailRequest, InvoiceDetail>();

            // // Item
            // this.CreateMap<Item, ItemLookup>()
            //     .ForMember(d => d.CodeName, s => s.MapFrom(o => $"{o.Code} - {o.Name}"))
            //     .ForMember(d => d.NameCategoryDescription, s => s.MapFrom(o => $"{o.Name} - {o.Category.Name} - {o.Description}"))
            //     .ForMember(d => d.CategoryName, s => s.MapFrom(o => o.Category.Name));

            // this.CreateMap<Item, ItemSummary>()
            //     .ForMember(d => d.CategoryName, s => s.MapFrom(o => o.Category.Name));

            // this.CreateMap<SaveItemRequest, Item>();

            // // Order
            // this.CreateMap<Order, OrderSummary>()
            //     .ForMember(d => d.ProvinceName, s => s.MapFrom(o => o.Customer.Province.Name))
            //     .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToString("d")))
            //     .ForMember(d => d.DueDate, s => s.MapFrom(o => o.DueDate.HasValue ? o.DueDate.Value.ToString("d") : string.Empty));

            // this.CreateMap<SaveOrderRequest, Order>();

            // this.CreateMap<Order, OrderLookup>();

            // // Order Detail
            // this.CreateMap<OrderDetail, OrderDetailSummary>()
            //     .ForMember(d => d.ItemCode, s => s.MapFrom(o => o.Item.Code))
            //     .ForMember(d => d.Item, s => s.MapFrom(o => o.Item.Name))
            //     .ForMember(d => d.Unit, s => s.MapFrom(o => o.Item.Unit))
            //     .ForMember(d => d.Description, s => s.MapFrom(o => o.Item.Description))
            //     .ForMember(d => d.Category, s => s.MapFrom(o => o.Item.Category.Name));

            // this.CreateMap<SaveOrderDetailRequest, OrderDetail>();

            // this.CreateMap<OrderDetail, OrderDetailLookup>()
            //     .ForMember(d => d.ItemName, s => s.MapFrom(o => o.Item.Name))
            //     .ForMember(d => d.Unit, s => s.MapFrom(o => o.Item.Unit))
            //     .ForMember(d => d.ItemCodeName, s => s.MapFrom(o => $"{o.Item.Code} - {o.Item.Name}"))
            //     .ForMember(d => d.ItemCodeNameOrder, s => s.MapFrom(o => $"{o.Item.Code} - {o.Item.Name} ({o.Order.Code})"))
            //     .ForMember(d => d.Category, s => s.MapFrom(o => o.Item.Category.Name))
            //     .ForMember(d => d.QuantityDelivered, s => s.MapFrom(o => o.QuantityDelivered));

            // // Reference
            // this.CreateMap<Reference, ReferenceLookup>();

            // this.CreateMap<Reference, ReferenceSummary>()
            //     .ForMember(d => d.ReferenceTypeCode, s => s.MapFrom(o => o.ReferenceType.Code));

            // this.CreateMap<SaveReferenceRequest, Reference>();

            // // ReferenceType
            // this.CreateMap<ReferenceType, ReferenceTypeSummary>();

            // this.CreateMap<SaveReferenceTypeRequest, ReferenceType>();

            // // Sales Quote
            // this.CreateMap<SalesQuote, SalesQuoteSummary>()
            //     .ForMember(d => d.QuoteNumber, s => s.MapFrom(o => o.QuoteNumber.ToString()))
            //     .ForMember(d => d.CustomerName, s => s.MapFrom(o => o.Customer.Name))
            //     .ForMember(d => d.CustomerAddress, s => s.MapFrom(o => $"{o.Customer.Address}, {o.Customer.City.Name} {o.Customer.Province.Name}"))
            //     .ForMember(d => d.CustomerContact, s => s.MapFrom(o => o.Customer.ContactNumber));

            // this.CreateMap<SaveSalesQuoteRequest, SalesQuote>();

            // // Sales Quote Detail
            // this.CreateMap<SalesQuoteDetail, SalesQuoteDetailSummary>()
            //     .ForMember(d => d.ItemCode, s => s.MapFrom(o => o.Item.Code))
            //     .ForMember(d => d.Item, s => s.MapFrom(o => o.Item.Name))
            //     .ForMember(d => d.Unit, s => s.MapFrom(o => o.Item.Unit))
            //     .ForMember(d => d.Description, s => s.MapFrom(o => o.Item.Description))
            //     .ForMember(d => d.Category, s => s.MapFrom(o => o.Item.Category.Name));

            // this.CreateMap<SaveSalesQuoteDetailRequest, SalesQuoteDetail>();

            // // User
            // this.CreateMap<User, UserSummary>()
            //     .ForMember(d => d.Admin, o => o.MapFrom(s => s.AccessRights.Contains("admin")))
            //     .ForMember(d => d.CanView, o => o.MapFrom(s => s.AccessRights.Contains("canView")))
            //     .ForMember(d => d.CanWrite, o => o.MapFrom(s => s.AccessRights.Contains("canWrite")))
            //     .ForMember(d => d.CanDelete, o => o.MapFrom(s => s.AccessRights.Contains("canDelete")));

            // this.CreateMap<SaveUserRequest, User>()
            //     .ForMember(d => d.PasswordHash, o => o.Ignore());
        }
    }
}
