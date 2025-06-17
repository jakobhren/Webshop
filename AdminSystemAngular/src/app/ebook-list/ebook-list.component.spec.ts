import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EbookListComponent } from './ebook-list.component';

describe('EbookListComponent', () => {
  let component: EbookListComponent;
  let fixture: ComponentFixture<EbookListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EbookListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EbookListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
