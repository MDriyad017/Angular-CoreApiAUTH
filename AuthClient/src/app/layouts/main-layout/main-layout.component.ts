import { Component, OnInit, HostListener } from '@angular/core';
import { Router, RouterOutlet, RouterModule } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { UserService } from '../../shared/services/user.service';
import { CommonModule } from '@angular/common';
import { HideIfClaimsNotMetDirective } from '../../directives/hide-if-claims-not-met.directive';
import { claimReq } from '../../shared/utils/claimReq-utils';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet, CommonModule, RouterModule, HideIfClaimsNotMetDirective],
  templateUrl: './main-layout.component.html',
  styles: ``
})
export class MainLayoutComponent implements OnInit {

  fullName: string = '';
  isSidebarOpen: boolean = true;
  isUserDropdownOpen: boolean = false;
  isHovering: boolean = false;
  claimReq = claimReq;

  private hoverTimeout: any;

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
    if (this.isMobileScreen()) {
      this.isSidebarOpen = false;
    }

    // Close dropdown when clicking outside
    document.addEventListener('click', this.handleClickOutside.bind(this));
  }

  toggleUserDropdown() {
    this.isUserDropdownOpen = !this.isUserDropdownOpen;
  }

  onMouseEnterDropdown() {
    this.isHovering = true;
    // Clear any existing timeout
    if (this.hoverTimeout) {
      clearTimeout(this.hoverTimeout);
    }
    this.isUserDropdownOpen = true;
  }

  onMouseLeaveDropdown() {
    this.isHovering = false;
    // Add a small delay before closing to allow for moving to dropdown menu
    this.hoverTimeout = setTimeout(() => {
      if (!this.isHovering) {
        this.isUserDropdownOpen = false;
      }
    }, 300);
  }

  handleClickOutside(event: Event) {
    const target = event.target as HTMLElement;
    if (!target.closest('.user-dropdown')) {
      this.isUserDropdownOpen = false;
    }
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  onMenuItemClick() {
    if (this.isMobileScreen()) {
      this.isSidebarOpen = false;
    }
  }

  isMobileScreen(): boolean {
    return window.innerWidth < 992;
  }

  onProfileClick() {
    this.isUserDropdownOpen = false;
    // Add your profile navigation logic here
    console.log('Profile clicked');
    // this.router.navigate(['/profile']);
  }

  onLogoutClick() {
    this.isUserDropdownOpen = false;
    this.onLogout();
  }

  onLogout() {
    this.authService.deleteToken();
    this.router.navigateByUrl('/login');
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    if (this.isMobileScreen()) {
      this.isSidebarOpen = false;
    } else {
      this.isSidebarOpen = true;
    }
  }

  ngOnDestroy() {
    document.removeEventListener('click', this.handleClickOutside.bind(this));
    if (this.hoverTimeout) {
      clearTimeout(this.hoverTimeout);
    }
  }
}