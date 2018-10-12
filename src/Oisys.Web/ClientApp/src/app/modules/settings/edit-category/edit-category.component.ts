import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../../shared/services/category.service'
import { Category } from '../../../shared/models/category';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent {
  category: Category = new Category();

  constructor(private categoryService: CategoryService) { }

  saveCategory(categoryName: string) {
    if(categoryName) {
      this.category.name = categoryName;
      this.categoryService.saveCategory(this.category);
      this.clearCategory();
      alert("SVE!");
    }
  }

  loadCategory(category: Category) {
    this.category = category;
  };

  clearCategory() {
    this.category = new Category();
  }
}
