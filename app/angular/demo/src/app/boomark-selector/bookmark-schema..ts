import { MarkSpec, Schema } from 'prosemirror-model';

import { schema as defaultSchema } from 'ngx-editor/schema'; // import the full default schema

const bookmarkMark: MarkSpec = {
  attrs: { type: {} },
  inclusive: false,
  toDOM(mark) {
    const value = mark.attrs['type'];
    return [
      'span',
      { 'data-bookmark': value, class: 'bookmark' },
      `{{${value}}}`
    ];
  },
  parseDOM: [
    {
      tag: 'span[data-bookmark]',
      getAttrs(dom) {
        return { type: (dom as HTMLElement).getAttribute('data-bookmark') };
      },
    },
  ],
};

// take existing marks + add bookmark
const marks = defaultSchema.spec.marks.addToEnd('bookmark', bookmarkMark);

// reuse all default nodes
const nodes = defaultSchema.spec.nodes;

export const bookmarkSchema = new Schema({ nodes, marks });


