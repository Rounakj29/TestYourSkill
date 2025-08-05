import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SkillverifyComponent } from './skillverify/skillverify.component';
import { ToolbarModule } from 'primeng/toolbar';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { NgClass } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { CommonModule } from '@angular/common';
import { PasswordModule } from 'primeng/password';

import { InputTextModule } from 'primeng/inputtext';
import { Card } from "primeng/card";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    SkillverifyComponent,
    ToolbarModule,
    ButtonModule,
    ToggleSwitchModule,
    FormsModule,
    NgClass,
    DialogModule,
    InputTextModule,
    CommonModule,
    Card,
    PasswordModule
],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  title = 'frontend';
  items: AppComponent[] | undefined;
  checked: boolean = false;
  visible: boolean = false;
  verifyCode: string = '';
  accessGranted: boolean = false;
  errorMsg: string = '';
  showDialog() {
    this.visible = true;
    this.errorMsg = '';
    this.verifyCode = '';
  }
  ngOnInit(): void {
    console.log('Component has been initialized!');
    //wait for the DOM to be ready before toggling dark mode
    // setTimeout(() => {
    //   const element = document.querySelector('html');
    //   if (element) {
    //     element.classList.toggle('my-app-dark', this.checked);
    //   }
    // }, 10);
   // this.toggleDarkMode();
    // Perform initialization tasks here
  }
  toggleDarkMode() {
    this.checked = !this.checked;
    const element = document.querySelector('html');
    if (element) {
      element.classList.toggle('my-app-dark');
    }
    
  }
  async verifyAccess() {
    // Use GET with query parameter as per your backend
    const response = await fetch(
      `https://localhost:7101/api/UserRequest/VerifyAccess?value=${encodeURIComponent(
        this.verifyCode
      )}`,
      { method: 'POST' }
    );
    console.log(response);
    if (response.ok) {
      const result = await response.json();
      if (result === true) {
        this.accessGranted = true;
        this.visible = false;
      } else {
        this.errorMsg = 'Invalid code. Please try again.';
      }
    } else {
      this.errorMsg = 'Verification failed. Please try again.';
    }
  }
}
