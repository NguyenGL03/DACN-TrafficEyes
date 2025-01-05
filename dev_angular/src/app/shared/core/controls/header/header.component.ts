import { Component, Input, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'header',
    templateUrl: './header.component.html',
    encapsulation: ViewEncapsulation.None
})
export class HeaderComponent {
    @Input('title') title: string
}
