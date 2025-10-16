import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouteCalculatorComponent } from './components/route-calculator/route-calculator.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouteCalculatorComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Short Route Optimizer';
}