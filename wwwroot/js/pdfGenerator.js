// pdfGenerator.js
function generatePDF(tableId, fileName, hiddenColumns = []) {
    // Clone the table
    var originalTable = document.getElementById(tableId);
    var clonedTable = originalTable.cloneNode(true);

    // Remove the specified columns
    if (hiddenColumns.length > 0) {
        var rows = clonedTable.rows;
        for (var i = 0; i < rows.length; i++) {
            for (var j = hiddenColumns.length - 1; j >= 0; j--) {
                if (rows[i].cells[hiddenColumns[j]]) {
                    rows[i].deleteCell(hiddenColumns[j]);
                }
            }
        }
    }

    // Create a temporary container for the modified table
    var tempDiv = document.createElement('div');
    tempDiv.style.position = 'absolute';
    tempDiv.style.top = '-9999px';
    tempDiv.appendChild(clonedTable);
    document.body.appendChild(tempDiv);

    html2canvas(clonedTable).then(canvas => {
        const { jsPDF } = window.jspdf;
        var imgData = canvas.toDataURL('image/png');
        var pdf = new jsPDF('p', 'mm', 'a4');
        var imgWidth = 210;
        var pageHeight = 295;
        var imgHeight = canvas.height * imgWidth / canvas.width;
        var heightLeft = imgHeight;
        var position = 0;

        pdf.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
        heightLeft -= pageHeight;

        while (heightLeft >= 0) {
            position = heightLeft - imgHeight;
            pdf.addPage();
            pdf.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
            heightLeft -= pageHeight;
        }

        pdf.save(fileName);
        // Remove the temporary container
        document.body.removeChild(tempDiv);
    }).catch(error => {
        console.error('Error generating PDF:', error);
    });
}
