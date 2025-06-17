import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BuyEbookComponent } from './buy-ebook.component';
import { RouterTestingModule } from '@angular/router/testing';
import { EbookService } from '../services/ebook.service';
import { OrderService } from '../services/order.service';
import { OrderItemService } from '../services/order-item.service';
import { of } from 'rxjs';

describe('BuyEbookComponent', () => {
  let component: BuyEbookComponent;
  let fixture: ComponentFixture<BuyEbookComponent>;

  const mockEbookService = {
    getEbook: jasmine.createSpy('getEbook').and.returnValue(of({
      ebookId: 1,
      price: 10,
      title: 'Test Book',
      author: 'Author',
      publicationYear: 2020,
      file_url: '',
      image_url: '',
      categoryId: 1
    }))
  };

  const mockOrderService = {
    createOrder: jasmine.createSpy('createOrder').and.returnValue(of(1))
  };

  const mockOrderItemService = {
    createOrderItem: jasmine.createSpy('createOrderItem').and.returnValue(of({}))
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [BuyEbookComponent],
      providers: [
        { provide: EbookService, useValue: mockEbookService },
        { provide: OrderService, useValue: mockOrderService },
        { provide: OrderItemService, useValue: mockOrderItemService },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(BuyEbookComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch ebook on init', () => {
    component.ngOnInit();
    expect(mockEbookService.getEbook).toHaveBeenCalled();
    expect(component.ebook.title).toEqual('Test Book');
  });

  it('should trigger purchase and navigate to profile', () => {
    localStorage.setItem('email', 'test@example.com');
    component.ebook = { ebookId: 1, price: 10, title: '', author: '', publicationYear: 2020, file_url: '', image_url: '', categoryId: 1 };

    component.purchase();

    expect(mockOrderService.createOrder).toHaveBeenCalled();
    expect(component.purchaseSuccess).toBeTrue();
  });
});
