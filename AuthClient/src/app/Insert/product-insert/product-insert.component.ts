import { Component, OnInit, inject } from '@angular/core';
import { FormsModule, NgForm, NgModel } from '@angular/forms';
import { Router } from "@angular/router";
import { CommonModule } from '@angular/common';
// PrimeNG imports
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { RippleModule } from 'primeng/ripple';

import { ProductService } from '../../shared/services/product.service';

@Component({
  selector: 'app-product-insert',
  standalone: true,
  imports: [CommonModule,FormsModule,InputTextModule,ButtonModule,ToastModule,RippleModule
  ],
  templateUrl: './product-insert.component.html',
  providers: [MessageService] // MessageService is already provided here
})
export class ProductInsertComponent implements OnInit {
  // Using inject() for services
  private productService = inject(ProductService);
  private messageService = inject(MessageService); // MessageService is injected here
  private router = inject(Router);

  ngOnInit(): void {
    // Check if user is authenticated before allowing access
  }

  formSubmitted: boolean = false;
  isSubmitting: boolean = false;

  // Individual properties
  productCode: string = '';
  productName: string = '';

  // Error checking method
  hasError(control: NgModel): boolean {
    return !!control.invalid && (this.formSubmitted || !!control.touched || !!control.dirty);
  }

  onSubmit(form: NgForm) {
    this.formSubmitted = true;

    if (form.valid) {
      this.isSubmitting = true;
      
      const productData = {
        ProductCode: this.productCode,
        ProductName: this.productName
      };

      this.productService.insertProduct(productData).subscribe({
        next: (res: any) => {
          this.isSubmitting = false;
          
          if (res.isSuccess) {
            // This is where you show success toast - ADD styleClass HERE
            this.messageService.add({
              severity: 'success',
              summary: 'Insert Success',
              detail: res.message,
              life: 1500,
              styleClass: 'small-toast-message' // Add custom class here
            });
            
            // Reset form and properties
            this.productCode = '';
            this.productName = '';
            form.resetForm();
            this.formSubmitted = false;
            
          } else {
            // This is where you show error toast - ADD styleClass HERE
            this.messageService.add({
              severity: 'error',
              summary: 'Insert Failed',
              detail: res.message,
              life: 1500,
              styleClass: 'small-toast-message' // Add custom class here
            });
          }
        },
        error: (err: any) => {
          this.isSubmitting = false;
          
          let errorMessage = 'An error occurred while adding the product';
          
          if (err.status === 400) {
            errorMessage = 'Invalid product data';
          } else if (err.status === 409) {
            errorMessage = 'Product code already exists';
          }

          // This is where you show error toast - ADD styleClass HERE
          this.messageService.add({
            severity: 'error',
            summary: 'Insert Failed',
            detail: errorMessage,
            life: 1500,
            styleClass: 'small-toast-message' // Add custom class here
          });
          
          console.log('Error during product insertion:\n', err);
        }
      });
    } else {
      // This is where you show validation error toast - ADD styleClass HERE
      this.messageService.add({
        severity: 'error',
        summary: 'Validation Error',
        detail: 'Please fill all required fields correctly',
        life: 1500,
        styleClass: 'small-toast-message' // Add custom class here
      });
    }
  }
}