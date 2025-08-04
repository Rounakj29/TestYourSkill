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

@Component({
    selector: 'app-skillverify',
    standalone: true,
    imports: [ButtonModule, CardModule, InputTextModule, InputNumberModule, Textarea, FormsModule, HttpClientModule, CommonModule],
    templateUrl: './skillverify.component.html',
    styleUrl: './skillverify.component.css'
})
export class SkillverifyComponent {
      questions: any[] = [];
  error: string | null = null;

  constructor(private http: HttpClient) {}

  onSubmit(form: any) {
    if (form.invalid) return;
    const value = form.value;
    const data = {
      TechStack: value.TechStack.split(',').map((s: string) => s.trim()).filter(Boolean),
      Experience: value.Experience ? parseInt(value.Experience, 10) : null,
      RoleOrCompany: value.RoleOrCompany,
      NoOfQuestion: value.NoOfQuestion ? parseInt(value.NoOfQuestion, 10) : null,
      AdditionalRequirements: value.AdditionalRequirements
    };

    this.http.post<any>('https://localhost:7101/api/UserRequest', data).subscribe({
      next: (result) => {
        this.error = null;
        this.questions = result.questions || [];
      },
      error: (err) => {
        this.error = 'Failed to fetch questions';
        this.questions = [];
      }
    });
  }
}
