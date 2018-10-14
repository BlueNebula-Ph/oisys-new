import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CategoryService } from '../../../shared/services/category.service'
import { Category } from '../../../shared/models/category';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit {
  @Input() category: Category;
  @Output() onCategorySaved: EventEmitter<Category> = new EventEmitter<Category>();

  constructor(private categoryService: CategoryService) { }

  ngOnInit() {
    this.clearCategory();
  }

  saveCategory(categoryName: string) {
    if (categoryName) {
      this.category.name = categoryName;
      this.categoryService
        .saveCategory(this.category)
        .subscribe(result => {
          this.clearCategory();
          this.onCategorySaved.emit(result);
        });
    }
  }

  clearCategory() {
    this.category = new Category();
  }
}
