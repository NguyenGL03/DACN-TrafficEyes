import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StyleService {
  private customStyles: { [key: string]: string } = {};

  setStyle(className: string, style: string) {
    this.customStyles[className] = style;
    this.applyStyles();
  }

  private applyStyles() {
    let styleElement = document.getElementById('dynamic-styles');
    if (!styleElement) {
      styleElement = document.createElement('style');
      styleElement.id = 'dynamic-styles';
      document.head.appendChild(styleElement);
    }

    styleElement.innerHTML = Object.entries(this.customStyles)
      .map(([className, style]) => `${className} { ${style} }`)
      .join(' ');
  }
}
