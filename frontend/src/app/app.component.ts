import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SkillverifyComponent } from "./skillverify/skillverify.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, SkillverifyComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'frontend';
}
