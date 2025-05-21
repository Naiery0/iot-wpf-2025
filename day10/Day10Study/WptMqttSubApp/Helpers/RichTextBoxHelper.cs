using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;

namespace WptMqttSubApp.Helpers
{
    public static class RichTextBoxHelper
    {
        // 바인딩할 문자열 프로퍼티
        public static readonly DependencyProperty BindableDocumentProperty =
            DependencyProperty.RegisterAttached(
                "BindableDocument",
                typeof(string),
                typeof(RichTextBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindableDocumentChanged));

        public static string GetBindableDocument(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableDocumentProperty);
        }

        public static void SetBindableDocument(DependencyObject obj, string value)
        {
            obj.SetValue(BindableDocumentProperty, value);
        }

        private static void OnBindableDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox richTextBox)
            {
                // 기존 문서 클리어
                richTextBox.Document.Blocks.Clear();
                // 새 문자열을 포함하는 Paragraph 추가
                richTextBox.Document.Blocks.Add(new Paragraph(new Run(e.NewValue as string ?? string.Empty)));
            }
        }
    }
}
