import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from "@angular/router";
import { CommonModule } from '@angular/common';
import { trigger, transition, style, animate } from '@angular/animations';
import { ProductService } from '../../shared/services/product.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-insert',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-insert.component.html',
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(40px)' }),
        animate(
          '600ms ease-out',
          style({ opacity: 1, transform: 'translateY(0)' })
        )
      ])
    ])
  ]
})
export class ProductInsertComponent implements OnInit {
  constructor(
    public formBuilder: FormBuilder,
    private productService: ProductService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Check if user is authenticated before allowing access
    // You can implement your own auth check here
  }

  isSubmitted: boolean = false;
  isSubmitting: boolean = false;

  productForm = this.formBuilder.group({
    productCode: ['', [Validators.required, Validators.minLength(3)]],
    productName: ['', [Validators.required, Validators.minLength(3)]]
  });

  hasDisplayableError(controlName: string): Boolean {
    const control = this.productForm.get(controlName);
    return (
      Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
    );
  }

  onSubmit() {
    this.isSubmitted = true;

    if (this.productForm.valid) {
      this.isSubmitting = true;
      
      const productData = {
        ProductCode: this.productForm.value.productCode,
        ProductName: this.productForm.value.productName
      };

      this.productService.insertProduct(productData).subscribe({
        next: (res: any) => {
          this.isSubmitting = false;
          
          if (res.isSuccess) {
            this.toastr.success(res.message, 'Success');
            this.productForm.reset();
            this.isSubmitted = false;
            
            // Optionally navigate to product list or stay on form
            // this.router.navigateByUrl('/products');
          } else {
            this.toastr.error(res.message, 'Insert Failed');
          }
        },
        error: err => {
          this.isSubmitting = false;
          
          if (err.status === 400) {
            this.toastr.error('Invalid product data', 'Insert Failed');
          } else if (err.status === 409) {
            this.toastr.error('Product code already exists', 'Duplicate Product');
          } else {
            this.toastr.error('An error occurred while adding the product', 'Insert Failed');
            console.log('Error during product insertion:\n', err);
          }
        }
      });
    }
  }
}