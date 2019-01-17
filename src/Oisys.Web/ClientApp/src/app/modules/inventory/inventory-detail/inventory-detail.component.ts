import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { InventoryService } from '../../../shared/services/inventory.service';
import { Item } from '../../../shared/models/item';

@Component({
  selector: 'app-inventory-detail',
  templateUrl: './inventory-detail.component.html',
  styleUrls: ['./inventory-detail.component.css']
})
export class InventoryDetailComponent implements OnInit {
  item: Item = new Item();

  constructor(private route: ActivatedRoute, private inventoryService: InventoryService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      var id = params.get("id");
      this.inventoryService
        .getItemById(parseInt(id))
        .subscribe(result => {
          this.item = result;
        });
    });
  }
}
