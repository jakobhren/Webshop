import { Component, Input } from '@angular/core';
import { Ebook } from '../model/ebook';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-ebook',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './ebook.component.html',
  styleUrl: './ebook.component.css'
})
export class EbookComponent {

  mode = 0;
  @Input() ebook? : Ebook;
  
  
  /*      ebookId: 1,
      title: 'Jane',
      author: 'Doe',
      price: 1,
      file_url: 'https://ia800204.us.archive.org/31/items/solitarysummer00elizgoog/solitarysummer00elizgoog.pdf' ,
      categoryId: 1,
 */     

}
