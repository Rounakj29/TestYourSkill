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
import { ToastModule } from 'primeng/toast';
import { Ripple } from 'primeng/ripple';
import { MessageService } from 'primeng/api';

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
    Knob,
    ToastModule,
    Ripple
  ],
  templateUrl: './skillverify.component.html',
  styleUrl: './skillverify.component.css',
  providers: [MessageService]
})
export class SkillverifyComponent {
  questions: any[] = [];
  error: string | null = null;
  isLoading: boolean = false;
  constructor(private http: HttpClient,
    private cdr: ChangeDetectorRef,
    private messageService: MessageService
  ) {}
  difficultyLvl: number | undefined; //1 for easy, 2 for medium, 3 for hard 4 for mixed

  QuestionType: string | undefined;

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
    
    if (form.invalid) {
      this.error = 'Please fill out all required fields correctly.';
      this.isLoading = false;
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Please fill out all required fields correctly.' });
      this.cdr.detectChanges();
      return;
    }
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
      DifficultyLevel: value.DifficultyLvl,
      QuestionType: value.QuestionType,  
      AdditionalRequirements: value.AdditionalRequirements,
    };
    var localhostUrl = 'https://localhost:7101/api/UserRequest';
    var azureUrl = 'https://skill-ish-bnf8dxejg7czhmbw.eastus2-01.azurewebsites.net/api/UserRequest';
    this.http
      .post<any>(azureUrl, data)
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
