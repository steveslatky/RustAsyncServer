DOCUMENTS = requirements.pdf checkerboard_spec.pdf design_doc.pdf test_doc.pdf

all: $(DOCUMENTS) 
.PHONY: clean

# Use Pandoc to convert Markdown -> LaTeX PDF
%.pdf: %.md
	pandoc --template latex_template.tex --top-level-division=section --toc -o $@ $<

clean:
	-rm $(DOCUMENTS)
