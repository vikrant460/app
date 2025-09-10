import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgxEditorComponent, NgxEditorMenuComponent, Editor, Toolbar } from 'ngx-editor';
import { BoomarkSelector } from '../boomark-selector/boomark-selector';
import { CommonModule } from '@angular/common';
import { bookmarkSchema } from '../boomark-selector/bookmark-schema.';
@Component({
  selector: 'app-ngx-texteditor',
  imports: [NgxEditorComponent, NgxEditorMenuComponent, BoomarkSelector, CommonModule],
  templateUrl: './ngx-texteditor.html',
  styleUrl: './ngx-texteditor.css'
})
export class NgxTexteditor implements OnInit, OnDestroy {
  editor!: Editor;
  toolbar: Toolbar = [
    ['bold', 'italic'],
    ['ordered_list', 'bullet_list'],
    [{ heading: ['h1', 'h2', 'h3', 'h4', 'h5', 'h6'] }],
    ['link'],
    ['underline', 'strike'],
    ['code', 'blockquote'],
    ['text_color', 'background_color'],
    ['horizontal_rule', 'format_clear', 'indent', 'outdent'],
    ['superscript', 'subscript'],
    ['undo', 'redo'],
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
  output: object = {};
  ngOnInit(): void {
    this.editor = new Editor({
      schema: bookmarkSchema,
      content: this.paragraphDoc,
      
    });
    this.editor.valueChanges.subscribe(value=> {
      console.log(value);
      this.output = value;
    })
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
