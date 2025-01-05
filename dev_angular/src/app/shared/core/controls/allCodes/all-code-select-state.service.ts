import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AllCodeSelectStateService {
    ngModel: string | null = null;
    items: any[] = [];
}
