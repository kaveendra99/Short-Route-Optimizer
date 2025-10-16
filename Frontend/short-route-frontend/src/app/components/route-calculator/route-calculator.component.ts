import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouteService, ApiResponse, ShortestPathResult } from '../../services/route.service';

@Component({
  selector: 'app-route-calculator',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './route-calculator.component.html',
  styleUrl: './route-calculator.component.css'
})
export class RouteCalculatorComponent implements OnInit {
  nodes: string[] = [];
  startNode: string = '';
  endNode: string = '';
  
  result: ShortestPathResult | null = null;
  loading: boolean = false;
  error: string = '';

  constructor(private routeService: RouteService) { }

  ngOnInit(): void {
    this.loadNodes();
  }

  loadNodes(): void {
    this.routeService.getAllNodes().subscribe({
      next: (response: ApiResponse<string[]>) => {
        if (response.success) {
          this.nodes = response.data;
          if (this.nodes.length > 0) {
            this.startNode = this.nodes[0];
            this.endNode = this.nodes[this.nodes.length - 1];
          }
        }
      },
      error: (err: any) => {
        this.error = 'Failed to load nodes. Please check if the backend is running.';
        console.error('Error loading nodes:', err);
      }
    });
  }

  calculateRoute(): void {
    if (!this.startNode || !this.endNode) {
      this.error = 'Please select both start and end nodes';
      return;
    }

    this.loading = true;
    this.error = '';
    this.result = null;

    this.routeService.calculateShortestRoute(this.startNode, this.endNode).subscribe({
      next: (response: ApiResponse<ShortestPathResult>) => {
        this.loading = false;
        if (response.success && response.data.success) {
          this.result = response.data;
        } else {
          this.error = response.data.errorMessage || 'No path found between the selected nodes';
        }
      },
      error: (err: any) => {
        this.loading = false;
        this.error = 'Failed to calculate route. Please check your connection and try again.';
        console.error('Error calculating route:', err);
      }
    });
  }

  resetForm(): void {
    this.startNode = this.nodes[0] || '';
    this.endNode = this.nodes[this.nodes.length - 1] || '';
    this.result = null;
    this.error = '';
  }

  get pathDisplay(): string {
    if (!this.result?.shortestPath) return '';
    return this.result.shortestPath.join(' → ');
  }

  // Safe getters for template
  get totalDistance(): number {
    return this.result?.totalDistance ?? 0;
  }

  get pathLength(): number {
    return this.result?.shortestPath?.length ?? 0;
  }
}