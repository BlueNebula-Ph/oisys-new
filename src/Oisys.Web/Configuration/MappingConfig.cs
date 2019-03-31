using AutoMapper;
using OisysNew.DTO;
using OisysNew.DTO.CashVoucher;
using OisysNew.DTO.Category;
using OisysNew.DTO.CreditMemo;
using OisysNew.DTO.Customer;
using OisysNew.DTO.Dashboard;
using OisysNew.DTO.Delivery;
using OisysNew.DTO.Invoice;
using OisysNew.DTO.Item;
using OisysNew.DTO.Login;
using OisysNew.DTO.Order;
using OisysNew.DTO.Province;
using OisysNew.DTO.Report;
using OisysNew.DTO.SalesQuote;
using OisysNew.DTO.User;
using OisysNew.Extensions;
using OisysNew.Models;
using System.Linq;

namespace OisysNew.Configuration
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
            // Adjustment
            CreateMap<Adjustment, ItemAdjustmentSummary>()
                .ForMember(d => d.Item, s => s.MapFrom(o => o.Item.Name));

            CreateMap<SaveItemAdjustmentRequest, Adjustment>()
                .ForMember(d => d.Quantity, s => s.MapFrom(o => o.AdjustmentQuantity));

            // Cash Voucher
            CreateMap<CashVoucher, CashVoucherSummary>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()));
            CreateMap<SaveCashVoucherRequest, CashVoucher>();

            // Category
            CreateMap<Category, CategorySummary>();
            CreateMap<Category, CategoryLookup>();
            CreateMap<SaveCategoryRequest, Category>();

            // City
            CreateMap<City, CitySummary>()
                .ForMember(d => d.ProvinceName, s => s.MapFrom(o => o.Province.Name));
            CreateMap<City, CityLookup>();
            CreateMap<SaveCityRequest, City>();

            // Credit Memo
            CreateMap<CreditMemo, CreditMemoSummary>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.CustomerName, s => s.MapFrom(o => o.Customer.Name))
                .ForMember(d => d.CustomerAddress, s => s.MapFrom(o => $"{o.Customer.Address}, {o.Customer.City.Name} {o.Customer.Province.Name}"));

            CreateMap<CreditMemo, CreditMemoDetail>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.Customer, s => s.MapFrom(o => o.Customer));

            CreateMap<CreditMemo, CreditMemoLookup>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()));

            CreateMap<CreditMemo, CustomerTransactionSummary>()
                .ForMember(d => d.Code, s => s.MapFrom(o => $"Credit Memo # {o.Code.ToString()}"))
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.Type, s => s.MapFrom(o => "Credit Memo"))
                .ForMember(d => d.TotalAmount, s => s.MapFrom(o => o.TotalAmount))
                .ForMember(d => d.IsInvoiced, s => s.MapFrom(o => o.IsInvoiced));

            CreateMap<SaveCreditMemoRequest, CreditMemo>();

            // Credit Memo Line Item
            CreateMap<CreditMemoLineItem, CreditMemoDetailLineItem>()
                .ForMember(d => d.OrderLineItem, s => s.MapFrom(o => o.OrderLineItem))
                .ForMember(d => d.ShouldAddBackToInventory, s => s.MapFrom(o => o.ReturnedToInventory));

            CreateMap<SaveCreditMemoLineItemRequest, CreditMemoLineItem>()
                .ForMember(d => d.ReturnedToInventory, s => s.MapFrom(o => o.ShouldAddBackToInventory));

            // Customer
            CreateMap<Customer, CustomerLookup>()
                .ForMember(d => d.ProvinceName, s => s.MapFrom(o => o.Province.Name))
                .ForMember(d => d.CityName, s => s.MapFrom(o => o.City.Name))
                .ForMember(d => d.PriceListId, s => s.MapFrom(o => (int)o.PriceList));
            CreateMap<Customer, CustomerWithOrdersLookup>();

            CreateMap<Customer, CustomerSummary>()
                .ForMember(d => d.CityName, s => s.MapFrom(o => o.City.Name))
                .ForMember(d => d.ProvinceName, s => s.MapFrom(o => o.Province.Name))
                .ForMember(d => d.PriceList, s => s.MapFrom(o => o.PriceList.GetDisplayName()));

            CreateMap<Customer, CustomerDetail>()
                .ForMember(d => d.PriceListId, s => s.MapFrom(o => (int)o.PriceList))
                .ForMember(d => d.Province, s => s.MapFrom(o => o.Province))
                .ForMember(d => d.City, s => s.MapFrom(o => o.City));

            CreateMap<SaveCustomerRequest, Customer>()
                .ForMember(d => d.PriceList, s => s.MapFrom(o => o.PriceListId))
                .ForMember(d => d.Keywords, s => s.MapFrom(o => $"{o.Name} {o.Address} {o.ContactNumber} {o.ContactPerson}"));

            // Customer Transaction
            CreateMap<CustomerTransaction, CustomerTransactionSummary>();

            // Delivery
            CreateMap<Delivery, DeliverySummary>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.ProvinceName, s => s.MapFrom(o => o.Province.Name))
                .ForMember(d => d.CityName, s => s.MapFrom(o => o.City.Name));

            CreateMap<Delivery, DeliveryDetail>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.Province, s => s.MapFrom(o => o.Province))
                .ForMember(d => d.City, s => s.MapFrom(o => o.City));

            CreateMap<SaveDeliveryRequest, Delivery>();

            // Delivery Line Item
            CreateMap<DeliveryLineItem, DeliveryDetailLineItem>()
                .ForMember(d => d.Quantity, s => s.MapFrom(o => o.Quantity))
                .ForMember(d => d.OrderCode, s => s.MapFrom(o => o.OrderLineItem.Order.Code))
                .ForMember(d => d.OrderDate, s => s.MapFrom(o => o.OrderLineItem.Order.Date.ToShortDateString()))
                .ForMember(d => d.ItemCode, s => s.MapFrom(o => o.OrderLineItem.Item.Code))
                .ForMember(d => d.ItemName, s => s.MapFrom(o => o.OrderLineItem.Item.Name))
                .ForMember(d => d.Unit, s => s.MapFrom(o => o.OrderLineItem.Item.Unit))
                .ForMember(d => d.CategoryName, s => s.MapFrom(o => o.OrderLineItem.Item.Category.Name))
                .ForMember(d => d.QuantityDelivered, s => s.MapFrom(o => o.OrderLineItem.QuantityDelivered))
                .ForMember(d => d.OrderLineItemId, s => s.MapFrom(o => o.OrderLineItem.Id))
                .ForMember(d => d.Customer, s => s.MapFrom(o => o.OrderLineItem.Order.Customer));

            CreateMap<SaveDeliveryLineItemRequest, DeliveryLineItem>();

            // Invoice
            CreateMap<Invoice, InvoiceSummary>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.CustomerName, s => s.MapFrom(o => o.Customer.Name))
                .ForMember(d => d.CustomerAddress, s => s.MapFrom(o => $"{o.Customer.Address}, {o.Customer.City.Name} {o.Customer.Province.Name}"));

            CreateMap<Invoice, InvoiceDetail>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.Customer, s => s.MapFrom(o => o.Customer));

            CreateMap<SaveInvoiceRequest, Invoice>();

            // Invoice Line Item
            CreateMap<InvoiceLineItem, InvoiceDetailLineItem>()
                .ForMember(d => d.Code, s => s.MapFrom(o => o.OrderId != 0 ? o.Order.Code : (o.CreditMemoId != 0 ? o.CreditMemo.Code : default(int))))
                .ForMember(d => d.Date, s => s.MapFrom(o => o.OrderId != 0 ? o.Order.Date.ToShortDateString() : (o.CreditMemoId != 0 ? o.CreditMemo.Date.ToShortDateString() : default(string))))
                .ForMember(d => d.TotalAmount, s => s.MapFrom(o => o.OrderId != 0 ? o.Order.TotalAmount : (o.CreditMemoId != 0 ? o.CreditMemo.TotalAmount : default(decimal))))
                .ForMember(d => d.Type, s => s.MapFrom(o => o.OrderId != 0 ? "Order" : "CreditMemo"));

            CreateMap<SaveInvoiceLineItemRequest, InvoiceLineItem>();

            // Item
            CreateMap<Item, ItemLookup>()
                .ForMember(d => d.CategoryName, s => s.MapFrom(o => o.Category.Name));

            CreateMap<Item, ItemSummary>()
                .ForMember(d => d.CategoryName, s => s.MapFrom(o => o.Category.Name));

            CreateMap<Item, ItemDetail>()
                .ForMember(d => d.Category, s => s.MapFrom(o => o.Category));

            CreateMap<Item, ItemCount>()
                .ForMember(d => d.Category, s => s.MapFrom(o => o.Category.Name));

            CreateMap<SaveItemRequest, Item>();

            // Item History
            CreateMap<ItemHistory, ItemHistorySummary>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()));

            // Order
            CreateMap<Order, OrderSummary>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.DueDate, s => s.MapFrom(o => o.DueDate.HasValue ? o.DueDate.Value.ToShortDateString() : string.Empty))
                .ForMember(d => d.CustomerName, s => s.MapFrom(o => o.Customer.Name))
                .ForMember(d => d.CustomerAddress, s => s.MapFrom(o => $"{o.Customer.Address}, {o.Customer.City.Name}, {o.Customer.Province.Name}"));

            CreateMap<Order, OrderDetail>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.DueDate, s => s.MapFrom(o => o.DueDate.HasValue ? o.DueDate.Value.ToShortDateString() : string.Empty))
                .ForMember(d => d.Customer, s => s.MapFrom(o => o.Customer));

            CreateMap<Order, OrderLookup>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()));

            CreateMap<Order, CustomerTransactionSummary>()
                .ForMember(d => d.Code, s => s.MapFrom(o => $"Order # {o.Code.ToString()}"))
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.Type, s => s.MapFrom(o => "Order"))
                .ForMember(d => d.TotalAmount, s => s.MapFrom(o => o.TotalAmount))
                .ForMember(d => d.IsInvoiced, s => s.MapFrom(o => o.IsInvoiced));

            CreateMap<Order, OrderReport>()
                .ForMember(d => d.Code, s => s.MapFrom(o => $"Order # {o.Code.ToString()}"))
                .ForMember(d => d.Customer, s => s.MapFrom(o => o.Customer.Name))
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.DueDate, s => s.MapFrom(o => o.DueDate.HasValue ? o.DueDate.Value.ToShortDateString() : string.Empty))
                .ForMember(d => d.TotalAmount, s => s.MapFrom(o => o.TotalAmount));

            CreateMap<SaveOrderRequest, Order>();

            // Order Line Item
            CreateMap<OrderLineItem, OrderDetailLineItem>()
                .ForMember(d => d.Item, s => s.MapFrom(o => o.Item));

            CreateMap<SaveOrderLineItemRequest, OrderLineItem>();

            CreateMap<OrderLineItem, OrderLineItemLookup>()
                .ForMember(d => d.OrderCode, s => s.MapFrom(o => o.Order.Code))
                .ForMember(d => d.OrderDate, s => s.MapFrom(o => o.Order.Date.ToShortDateString()))
                .ForMember(d => d.ItemCode, s => s.MapFrom(o => o.Item.Code))
                .ForMember(d => d.ItemName, s => s.MapFrom(o => o.Item.Name))
                .ForMember(d => d.Unit, s => s.MapFrom(o => o.Item.Unit))
                .ForMember(d => d.CategoryName, s => s.MapFrom(o => o.Item.Category.Name))
                .ForMember(d => d.QuantityDelivered, s => s.MapFrom(o => o.QuantityDelivered))
                .ForMember(d => d.OrderLineItemId, s => s.MapFrom(o => o.Id))
                .ForMember(d => d.Customer, s => s.MapFrom(o => o.Order.Customer));

            CreateMap<OrderLineItem, ItemOrderSummary>()
                .ForMember(d => d.CustomerName, s => s.MapFrom(o => o.Order.Customer.Name))
                .ForMember(d => d.DateOrdered, s => s.MapFrom(o => o.Order.Date.ToShortDateString()))
                .ForMember(d => d.QuantityOrdered, s => s.MapFrom(o => o.Quantity));

            CreateMap<OrderLineItem, DashboardOrder>()
                .ForMember(d => d.Category, s => s.MapFrom(o => o.Item.Category.Name))
                .ForMember(d => d.Customer, s => s.MapFrom(o => o.Order.Customer.Name))
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Order.Date.ToShortDateString()))
                .ForMember(d => d.DueDate, s => s.MapFrom(o => o.Order.DueDate.HasValue ? o.Order.DueDate.Value.ToShortDateString() : string.Empty))
                .ForMember(d => d.ItemCode, s => s.MapFrom(o => o.Item.Code))
                .ForMember(d => d.ItemName, s => s.MapFrom(o => o.Item.Name))
                .ForMember(d => d.OrderCode, s => s.MapFrom(o => o.Order.Code))
                .ForMember(d => d.Quantity, s => s.MapFrom(o => o.Quantity))
                .ForMember(d => d.Unit, s => s.MapFrom(o => o.Item.Unit));

            // Province
            CreateMap<Province, ProvinceSummary>()
                .ForMember(d => d.Cities, s => s.MapFrom(o => o.Cities.OrderBy(c => c.Name)));
            CreateMap<Province, ProvinceLookup>()
                .ForMember(d => d.Cities, s => s.MapFrom(o => o.Cities.OrderBy(c => c.Name)));
            CreateMap<SaveProvinceRequest, Province>();

            // Sales Quote
            CreateMap<SalesQuote, SalesQuoteSummary>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.CustomerName, s => s.MapFrom(o => o.Customer.Name))
                .ForMember(d => d.CustomerAddress, s => s.MapFrom(o => $"{o.Customer.Address}, {o.Customer.City.Name} {o.Customer.Province.Name}"));

            CreateMap<SalesQuote, SalesQuoteDetail>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.Customer, s => s.MapFrom(o => o.Customer));

            CreateMap<SaveSalesQuoteRequest, SalesQuote>();

            // Sales Quote Detail
            CreateMap<SalesQuoteLineItem, SalesQuoteDetailLineItem>()
                .ForMember(d => d.Item, s => s.MapFrom(o => o.Item));

            CreateMap<SaveSalesQuoteLineItemRequest, SalesQuoteLineItem>();

            // User
            CreateMap<ApplicationUser, UserSummary>()
                .ForMember(d => d.Admin, o => o.MapFrom(s => s.AccessRights.Contains("admin")))
                .ForMember(d => d.CanView, o => o.MapFrom(s => s.AccessRights.Contains("canView")))
                .ForMember(d => d.CanWrite, o => o.MapFrom(s => s.AccessRights.Contains("canWrite")))
                .ForMember(d => d.CanDelete, o => o.MapFrom(s => s.AccessRights.Contains("canDelete")));

            CreateMap<ApplicationUser, UserDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Username))
                .ForMember(d => d.Fullname, o => o.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Admin, o => o.MapFrom(s => s.AccessRights.Contains("admin")))
                .ForMember(d => d.CanView, o => o.MapFrom(s => s.AccessRights.Contains("canView")))
                .ForMember(d => d.CanWrite, o => o.MapFrom(s => s.AccessRights.Contains("canWrite")))
                .ForMember(d => d.CanDelete, o => o.MapFrom(s => s.AccessRights.Contains("canDelete")))
                .ForMember(d => d.Token, o => o.Ignore());

            CreateMap<SaveUserRequest, ApplicationUser>()
                .ForMember(d => d.PasswordHash, o => o.Ignore());

            // // Customer Transaction
            // // TODO: Create method to compute running balance
            // CreateMap<SaveCustomerTrxRequest, CustomerTransaction>();

            // CreateMap<CustomerTransaction, CustomerTransactionSummary>()
            //     .ForMember(d => d.Customer, s => s.MapFrom(o => o.Customer.Name));
        }
    }
}
