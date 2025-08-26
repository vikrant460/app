import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgxEditorComponent, NgxEditorMenuComponent, Editor, Toolbar } from 'ngx-editor';
import { BoomarkSelector } from '../boomark-selector/boomark-selector';
@Component({
  selector: 'app-ngx-texteditor',
  imports: [NgxEditorComponent, NgxEditorMenuComponent, BoomarkSelector],
  templateUrl: './ngx-texteditor.html',
  styleUrl: './ngx-texteditor.css'
})
export class NgxTexteditor implements OnInit, OnDestroy {
  editor!: Editor;
  toolbar: Toolbar = [
    // default value
    ['bold', 'italic'],
    ['ordered_list', 'bullet_list'],
    [{ heading: ['h1', 'h2', 'h3', 'h4', 'h5', 'h6'] }],
    ['link']
  ];
  html!: '';
  private paragraphDoc = {
    "type": "doc",
    "content": [
      {
        "type": "paragraph",
        "content": [
          {
            "type": "text",
            "text": "Hello Ngx Editor!"
          }
        ]
      }
    ]
  }
  ngOnInit(): void {
    this.editor = new Editor({
      content: this.paragraphDoc,

    });
  }

  // make sure to destory the editor
  ngOnDestroy(): void {
    this.editor.destroy();
  }
  insertText(event: Event): void {
    const text = (event.target as HTMLTextAreaElement).value;
    this.editor.commands
      .insertText(text)
      .focus() // Optional: Focus the editor after insertion
      .exec();
  }
}
