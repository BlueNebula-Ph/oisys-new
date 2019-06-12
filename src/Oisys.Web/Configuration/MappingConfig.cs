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
using System;
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
                .ForMember(d => d.Item, s => s.MapFrom(o => $"{o.Item.Code} - {o.Item.Name}"))
                .ForMember(d => d.AdjustmentQuantity, s => s.MapFrom(o => o.Quantity))
                .ForMember(d => d.AdjustmentDate, s => s.MapFrom(o => o.AdjustmentDate.ToShortDateString()));

            CreateMap<SaveItemAdjustmentRequest, Adjustment>()
                .ForMember(d => d.Quantity, s => s.MapFrom(o => o.AdjustmentQuantity))
                .ForMember(d => d.AdjustmentDate, s => s.MapFrom(o => DateTime.Now));

            // Cash Voucher
            CreateMap<CashVoucher, CashVoucherSummary>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.ToBase64String(o.RowVersion)));

            CreateMap<SaveCashVoucherRequest, CashVoucher>()
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.FromBase64String(o.RowVersion)));

            // Category
            CreateMap<Category, CategorySummary>()
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.ToBase64String(o.RowVersion)));

            CreateMap<Category, CategoryLookup>();

            CreateMap<SaveCategoryRequest, Category>()
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.FromBase64String(o.RowVersion)));

            // City
            CreateMap<City, CitySummary>()
                .ForMember(d => d.ProvinceName, s => s.MapFrom(o => o.Province.Name))
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.ToBase64String(o.RowVersion)));

            CreateMap<City, CityLookup>();

            CreateMap<SaveCityRequest, City>()
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.FromBase64String(o.RowVersion))); ;

            // Credit Memo
            CreateMap<CreditMemo, CreditMemoSummary>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.CustomerName, s => s.MapFrom(o => o.Customer.Name))
                .ForMember(d => d.CustomerAddress, s => s.MapFrom(o => $"{o.Customer.Address}, {o.Customer.City.Name} {o.Customer.Province.Name}"));

            CreateMap<CreditMemo, CreditMemoDetail>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.Customer, s => s.MapFrom(o => o.Customer))
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.ToBase64String(o.RowVersion)));

            CreateMap<CreditMemo, CreditMemoLookup>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()));

            CreateMap<CreditMemo, CustomerTransactionSummary>()
                .ForMember(d => d.Code, s => s.MapFrom(o => $"Credit Memo # {o.Code.ToString()}"))
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.Type, s => s.MapFrom(o => "Credit Memo"))
                .ForMember(d => d.TotalAmount, s => s.MapFrom(o => o.TotalAmount))
                .ForMember(d => d.IsInvoiced, s => s.MapFrom(o => o.IsInvoiced));

            CreateMap<SaveCreditMemoRequest, CreditMemo>()
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.FromBase64String(o.RowVersion)));

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
                .ForMember(d => d.City, s => s.MapFrom(o => o.City))
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.ToBase64String(o.RowVersion)));

            CreateMap<SaveCustomerRequest, Customer>()
                .ForMember(d => d.PriceList, s => s.MapFrom(o => o.PriceListId))
                .ForMember(d => d.Keywords, s => s.MapFrom(o => $"{o.Name} {o.Address} {o.ContactNumber} {o.ContactPerson}"))
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.FromBase64String(o.RowVersion)));

            // Customer Transaction
            CreateMap<CustomerTransaction, CustomerTransactionSummary>();

            // Delivery
            CreateMap<Delivery, DeliverySummary>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.DeliveryAreas, s => s.MapFrom(o => o.LineItems.Count != 0 ? 
                    string.Join(" | ", o.LineItems.GroupBy(a => new { Province = a.OrderLineItem.Order.Customer.Province.Name, City = a.OrderLineItem.Order.Customer.City.Name }).Select(a => $"{a.Key.City}, {a.Key.Province}")) : 
                    string.Empty));

            CreateMap<Delivery, DeliveryDetail>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.ToBase64String(o.RowVersion)));

            CreateMap<SaveDeliveryRequest, Delivery>()
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.FromBase64String(o.RowVersion)));

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
                .ForMember(d => d.Customer, s => s.MapFrom(o => o.Customer))
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.ToBase64String(o.RowVersion)));

            CreateMap<SaveInvoiceRequest, Invoice>()
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.FromBase64String(o.RowVersion)));

            // Invoice Line Item
            CreateMap<InvoiceLineItem, InvoiceDetailLineItem>()
                .ForMember(d => d.Code, s => s.MapFrom(o => o.OrderId.HasValue ? o.Order.Code : (o.CreditMemoId.HasValue ? o.CreditMemo.Code : default(int))))
                .ForMember(d => d.Date, s => s.MapFrom(o => o.OrderId.HasValue ? o.Order.Date.ToShortDateString() : (o.CreditMemoId.HasValue ? o.CreditMemo.Date.ToShortDateString() : default(string))))
                .ForMember(d => d.Type, s => s.MapFrom(o => o.OrderId.HasValue ? "Order" : "CreditMemo"))
                .ForMember(d => d.TotalAmount, s => s.MapFrom(o => o.TotalAmount));

            CreateMap<SaveInvoiceLineItemRequest, InvoiceLineItem>();

            // Item
            CreateMap<Item, ItemLookup>()
                .ForMember(d => d.CategoryName, s => s.MapFrom(o => o.Category.Name));

            CreateMap<Item, ItemSummary>()
                .ForMember(d => d.CategoryName, s => s.MapFrom(o => o.Category.Name));

            CreateMap<Item, ItemDetail>()
                .ForMember(d => d.Category, s => s.MapFrom(o => o.Category))
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.ToBase64String(o.RowVersion)));

            CreateMap<Item, ItemCount>()
                .ForMember(d => d.Category, s => s.MapFrom(o => o.Category.Name));

            CreateMap<SaveItemRequest, Item>()
                .ForMember(d => d.StockQuantity, s => s.MapFrom(o => o.Quantity))
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.FromBase64String(o.RowVersion)));

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
                .ForMember(d => d.Customer, s => s.MapFrom(o => o.Customer))
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.ToBase64String(o.RowVersion)));

            CreateMap<Order, OrderLookup>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.TotalAmount, s => s.MapFrom(o => o.LineItems.AsQueryable().Sum(a => (a.QuantityDelivered - a.QuantityInvoiced) * a.DiscountedUnitPrice)));

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

            CreateMap<SaveOrderRequest, Order>()
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.FromBase64String(o.RowVersion)));

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
                .ForMember(d => d.Customer, s => s.MapFrom(o => o.Order.Customer))
                .ForMember(d => d.DiscountedUnitPrice, s => s.MapFrom(o => o.DiscountedUnitPrice));

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
                .ForMember(d => d.QuantityDelivered, s => s.MapFrom(o => o.QuantityDelivered))
                .ForMember(d => d.Unit, s => s.MapFrom(o => o.Item.Unit));

            // Province
            CreateMap<Province, ProvinceSummary>()
                .ForMember(d => d.Cities, s => s.MapFrom(o => o.Cities.OrderBy(c => c.Name)))
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.ToBase64String(o.RowVersion)));

            CreateMap<Province, ProvinceLookup>()
                .ForMember(d => d.Cities, s => s.MapFrom(o => o.Cities.OrderBy(c => c.Name)));

            CreateMap<SaveProvinceRequest, Province>()
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.FromBase64String(o.RowVersion)));

            // Sales Quote
            CreateMap<SalesQuote, SalesQuoteSummary>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.CustomerName, s => s.MapFrom(o => o.Customer.Name))
                .ForMember(d => d.CustomerAddress, s => s.MapFrom(o => $"{o.Customer.Address}, {o.Customer.City.Name} {o.Customer.Province.Name}"));

            CreateMap<SalesQuote, SalesQuoteDetail>()
                .ForMember(d => d.Date, s => s.MapFrom(o => o.Date.ToShortDateString()))
                .ForMember(d => d.Customer, s => s.MapFrom(o => o.Customer))
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.ToBase64String(o.RowVersion)));

            CreateMap<SaveSalesQuoteRequest, SalesQuote>()
                .ForMember(d => d.RowVersion, s => s.MapFrom(o => Convert.FromBase64String(o.RowVersion)));

            // Sales Quote Detail
            CreateMap<SalesQuoteLineItem, SalesQuoteDetailLineItem>()
                .ForMember(d => d.Item, s => s.MapFrom(o => o.Item));

            CreateMap<SaveSalesQuoteLineItemRequest, SalesQuoteLineItem>();

            // User
            CreateMap<ApplicationUser, UserSummary>()
                .ForMember(d => d.Admin, o => o.MapFrom(s => s.AccessRights.Contains("admin")))
                .ForMember(d => d.CanView, o => o.MapFrom(s => s.AccessRights.Contains("canView")))
                .ForMember(d => d.CanWrite, o => o.MapFrom(s => s.AccessRights.Contains("canWrite")))
                .ForMember(d => d.CanDelete, o => o.MapFrom(s => s.AccessRights.Contains("canDelete")))
                .ForMember(d => d.RowVersion, o => o.MapFrom(s => Convert.ToBase64String(s.RowVersion)));

            CreateMap<ApplicationUser, UserDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Username))
                .ForMember(d => d.Fullname, o => o.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Admin, o => o.MapFrom(s => s.AccessRights.Contains("admin")))
                .ForMember(d => d.CanView, o => o.MapFrom(s => s.AccessRights.Contains("canView")))
                .ForMember(d => d.CanWrite, o => o.MapFrom(s => s.AccessRights.Contains("canWrite")))
                .ForMember(d => d.CanDelete, o => o.MapFrom(s => s.AccessRights.Contains("canDelete")))
                .ForMember(d => d.Token, o => o.Ignore());

            CreateMap<SaveUserRequest, ApplicationUser>()
                .ForMember(d => d.PasswordHash, o => o.Ignore())
                .ForMember(d => d.RowVersion, o => o.MapFrom(s => Convert.FromBase64String(s.RowVersion)));
        }
    }
}
