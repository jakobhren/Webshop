import { Component, Input } from '@angular/core';
import { Category } from '../model/category';

@Component({
  selector: 'app-category',
  standalone: true,
  imports: [],
  templateUrl: './category.component.html',
  styleUrl: './category.component.css'
})
export class CategoryComponent {

  mode = 0;
 @Input() category?: Category = {

    categoryId: 1,
    categoryName: 'Science Fiction'
  
  }
}
