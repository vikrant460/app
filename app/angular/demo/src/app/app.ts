import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TiptapEditor } from './tiptap-editor/tiptap-editor';
import { NgxTexteditor } from "./ngx-texteditor/ngx-texteditor";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TiptapEditor, NgxTexteditor],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('demo');
}
