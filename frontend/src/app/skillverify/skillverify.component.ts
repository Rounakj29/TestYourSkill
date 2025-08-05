import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { Textarea } from 'primeng/textarea';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { FloatLabelModule } from 'primeng/floatlabel';
import { TextareaModule } from 'primeng/textarea';
import { ProgressSpinner } from 'primeng/progressspinner';
import { ChangeDetectorRef } from '@angular/core';
import { Knob } from 'primeng/knob';

interface City {
  name: string;
  code: string;
}
@Component({
  selector: 'app-skillverify',
  standalone: true,
  imports: [
    ButtonModule,
    CardModule,
    InputTextModule,
    InputNumberModule,
    Textarea,
    FormsModule,
    HttpClientModule,
    CommonModule,
    InputGroupModule,
    InputGroupAddonModule,
    FloatLabelModule,
    TextareaModule,
    ProgressSpinner,
    Knob
  ],
  templateUrl: './skillverify.component.html',
  styleUrl: './skillverify.component.css',
})
export class SkillverifyComponent {
  questions: any[] = [];
  error: string | null = null;
  isLoading: boolean = false;
  constructor(private http: HttpClient,
    private cdr: ChangeDetectorRef
  ) {}
  text1: string | undefined;

  text2: string | undefined;

  number: string | undefined;
  noOfQuestions: number = 0; 
  yearsOfExperience: number = 0;
  selectedCity: City | undefined;
  onNoOfQuestionsChange(event: any) {
    console.log('No of Questions changed:', event.value);
  }
  OnNo() {
    console.log('Knob value changed:', this.noOfQuestions);
    this.noOfQuestions = this.noOfQuestions - 1;
    this.cdr.detectChanges(); // Ensure UI reflects the change immediately
  }
  OnYes() {
    console.log('Knob value changed:', this.noOfQuestions);
    this.noOfQuestions = this.noOfQuestions + 1;
    this.cdr.detectChanges(); // Ensure UI reflects the change immediately
  }
  cities: City[] = [
    { name: 'New York', code: 'NY' },
    { name: 'Rome', code: 'RM' },
    { name: 'London', code: 'LDN' },
    { name: 'Istanbul', code: 'IST' },
    { name: 'Paris', code: 'PRS' },
  ];
  onSubmit(form: any) {
     this.isLoading = true;
     this.cdr.detectChanges(); // Ensure UI reflects the spinner change immediately
 
    debugger;
    if (form.invalid) return;
    
    const value = form.value;
    const data = {
      TechStack: value.TechStack.split(',')
        .map((s: string) => s.trim())
        .filter(Boolean),
      Experience: value.Experience ? parseInt(value.Experience, 10) : null,
      RoleOrCompany: value.RoleOrCompany,
      NoOfQuestion: value.NoOfQuestion
        ? parseInt(value.NoOfQuestion, 10)
        : null,
      AdditionalRequirements: value.AdditionalRequirements,
    };

    this.http
      .post<any>('https://localhost:7101/api/UserRequest', data)
      .subscribe({
        next: (result) => {
          this.isLoading = true;
          this.error = null;
          this.questions = result.questions || [];
          this.isLoading = false;
          this.cdr.detectChanges(); 
        },
        error: (err) => {
          this.error = 'Failed to fetch questions';
          this.questions = [];
          this.isLoading = false;
          this.cdr.detectChanges(); 
        },
      });
  }
}
