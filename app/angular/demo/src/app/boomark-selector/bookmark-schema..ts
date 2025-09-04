import { MarkSpec, Schema } from 'prosemirror-model';
import { schema } from 'ngx-editor/schema';
const bookmarkMark: MarkSpec = {
    inclusive: false,
    toDOM(mark) {
        const value = mark.attrs['type'];
        return [
            'span',
            { 'data-bookmark': value, class: 'bookmark' },
            0 // IMPORTANT: this tells ProseMirror to render the text content here
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
const marks = schema.spec.marks.addToEnd('bookmark', bookmarkMark);

// reuse all default nodes
const nodes = schema.spec.nodes;

export const bookmarkSchema = new Schema({ nodes, marks });


