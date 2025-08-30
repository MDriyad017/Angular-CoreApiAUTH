import { Component } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from "@angular/router";
import { CommonModule } from '@angular/common';
import {
  trigger,
  transition,
  style,
  animate,
  keyframes
} from '@angular/animations';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(40px)' }),
        animate(
          '600ms ease-out',
          style({ opacity: 1, transform: 'translateY(0)' })
        )
      ])
    ]),
    trigger('shake', [
      transition(':enter', [
        animate(
          '500ms ease-in',
          keyframes([
            style({ transform: 'translateX(-10px)', offset: 0.2 }),
            style({ transform: 'translateX(10px)', offset: 0.4 }),
            style({ transform: 'translateX(-10px)', offset: 0.6 }),
            style({ transform: 'translateX(10px)', offset: 0.8 }),
            style({ transform: 'translateX(0)', offset: 1 }),
          ])
        )
      ])
    ])
  ]
})
export class LoginComponent {
  constructor(public formBuilder: FormBuilder) {}

  isSubmitted: boolean = false;

  form = this.formBuilder.group({
    email: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });

  hasDisplayableError(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return (
      Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
    );
  }

  onSubmit() {
    this.isSubmitted = true;
    if (this.form.invalid) {
      return;
    }
    console.log(this.form.value);
  }
}
