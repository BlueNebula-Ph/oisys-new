import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { fromEvent, merge, Observable, of } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, map, startWith, switchMap, tap } from 'rxjs/operators';
import { Customer } from '../../../shared/models/customer';
import { SummaryItem } from '../../../shared/models/summary-item';
import { CustomerService } from '../../../shared/services/customer.service';
import { UtilitiesService } from '../../../shared/services/utilities.service';
import { Router } from '@angular/router';
import { trigger, state, transition, style, animate } from '@angular/animations';
import { SummaryColumn } from '../../../shared/models/summary-column';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0', display: 'none' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class CustomerListComponent implements AfterViewInit {
  summaryColumns: SummaryColumn[] = [
    new SummaryColumn("Name", "name"),
    new SummaryColumn("Email", "email"),
    new SummaryColumn("Address", "address"),
    new SummaryColumn("Contact #", "contactNumber"),
    new SummaryColumn("Contact Person", "contactPerson")
  ];
  displayedColumns: string[] = summaryColumns.map(col => { return col.propName; }).concat(['buttons']);
  dataSource = new MatTableDataSource();
  customers: Observable<SummaryItem<Customer>>;
  expandedElement: Customer;

  resultsLength = 0;
  isLoadingResults = false;

  constructor(private customerService: CustomerService, private util: UtilitiesService, private router: Router) { }

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('searchBox') input: ElementRef;

  ngAfterViewInit() {
    loadCustomers();
  };

  loadCustomers() {
    // If the user changes the sort order, reset back to the first page.
    sort.sortChange.subscribe(() => paginator.pageIndex = 0);

    merge(
      sort.sortChange,
      paginator.page,
      fromEvent(input.nativeElement, 'keyup')
        .pipe(
          debounceTime(350),
          distinctUntilChanged(),
          tap(() => {
            paginator.pageIndex = 0;
          })
        )
    )
      .pipe(
        startWith({}),
        switchMap(() => {
          isLoadingResults = true;
          return fetchCustomers();
        }),
        map(data => {
          // Flip flag to show that loading has finished.
          isLoadingResults = false;
          resultsLength = data.total_count;

          return data.items;
        }),
        catchError(() => {
          isLoadingResults = false;

          return of([]);
        })
      )
      .subscribe(data => dataSource.data = data);
  }

  fetchCustomers() {
    return customerService.getCustomers(
      paginator.pageIndex + 1,
      paginator.pageSize,
      sort.active,
      sort.direction,
      input.nativeElement.value);
  };

  addCustomer(id: number): void {
    var url = "/customers/edit/" + id;
    router.navigateByUrl(url);
  };

  //onEditCustomer(customerToEdit: Customer): void {
  //  selectedCustomer = customerToEdit;
  //};

  //onDeleteCustomer(id: number): void {
  //  if (confirm("Are you sure you want to delete this customer?")) {
  //    customerService.deleteCustomer(id).subscribe(() => {
  //      loadCustomers();
  //      util.openSnackBar("Customer deleted successfully.");
  //    });
  //  }
  //};

  //onCustomerSaved(customer: Customer): void {
  //  loadCustomers();
  //  util.openSnackBar("Customer saved successfully.");
  //};
}
