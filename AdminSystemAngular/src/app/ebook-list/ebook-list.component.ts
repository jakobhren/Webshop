import { Component, Input, OnInit } from '@angular/core';
import { Ebook } from '../model/ebook';
import { EbookComponent } from '../ebook/ebook.component';
import { EbookService } from '../services/ebook.service';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-ebook-list',
  standalone: true,
  imports: [EbookComponent, HttpClientModule, RouterModule, CommonModule],
  providers: [EbookService],
  templateUrl: './ebook-list.component.html',
  styleUrls: ['./ebook-list.component.css']
})
export class EbookListComponent implements OnInit {
 @Input() ebooks: Ebook[] = [];  

  constructor(private ebookService: EbookService) {}

  ngOnInit(): void {
    this.ebookService.getEbooks().subscribe(ebooks => {
      this.ebooks = ebooks;
    });
  }
}


  /*
  {
      ebookId: 1,
      title: 'Jane',
      author: 'Doe',
      price: 1,
      file_url: 'https://ia800204.us.archive.org/31/items/solitarysummer00elizgoog/solitarysummer00elizgoog.pdf' ,
      categoryId: 1,
      },

      {
        ebookId: 2,
        title: 'Jane',
        author: 'Doe',
        price: 1,
        file_url: 'https://ia800204.us.archive.org/31/items/solitarysummer00elizgoog/solitarysummer00elizgoog.pdf' ,
        categoryId: 1,
        },

        {
          ebookId: 3,
          title: 'Jane',
          author: 'Doe',
          price: 1,
          file_url: 'https://ia800204.us.archive.org/31/items/solitarysummer00elizgoog/solitarysummer00elizgoog.pdf' ,
          categoryId: 1,
          }
   ]; 

*/


