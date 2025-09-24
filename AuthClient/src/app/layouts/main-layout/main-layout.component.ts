import { Component, OnInit, HostListener } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { UserService } from '../../shared/services/user.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HideIfClaimsNotMetDirective } from '../../directives/hide-if-claims-not-met.directive';
import { claimReq } from '../../shared/utils/claimReq-utils';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet, CommonModule, RouterLink, HideIfClaimsNotMetDirective],
  templateUrl: './main-layout.component.html',
  styles: ``
})
export class MainLayoutComponent implements OnInit {

  fullName: string = '';
  isSidebarOpen: boolean = true;
  claimReq = claimReq;

  constructor(
    private router: Router,
    private authService: AuthService,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.userService.getUserProfile().subscribe({
      next: (res:any) => this.fullName = res.fullName,
      error:(err:any) => console.log('error while retriving User Profile data\n', err)
    });

    // Hide sidebar initially on small screens
    if (window.innerWidth < 800) {
      this.isSidebarOpen = false;
    }
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  onLogout() {
    this.authService.deleteToken();
    this.router.navigateByUrl('/login');
  }

  // Listen for screen resize
  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    if (event.target.innerWidth < 800) {
      this.isSidebarOpen = false;
    } else {
      this.isSidebarOpen = true;
    }
  }
}
