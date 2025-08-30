import { Component } from '@angular/core';
import { RegistrationComponent } from './registration/registration.component';
import { RouterOutlet } from '@angular/router'; // Import the component

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [RegistrationComponent, RouterOutlet],
  templateUrl: './user.component.html',
  styles: ``
})
export class UserComponent {

}
