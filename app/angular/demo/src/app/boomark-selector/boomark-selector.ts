import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Editor } from 'ngx-editor';
import { insertBookmark } from './bookmark-command';

@Component({
  selector: 'app-boomark-selector',
  imports: [CommonModule],
  templateUrl: './boomark-selector.html',
  styleUrl: './boomark-selector.css'
})
export class BoomarkSelector {
@Input() editor!: Editor;

  bookmarks = ['{{contact}}', '{{name}}', '{{email}}'];

  applyBookmark(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
    if (value && this.editor.view) {
      const { state, dispatch } = this.editor.view;
      insertBookmark(value)(state, dispatch); 
    }
  }
}
