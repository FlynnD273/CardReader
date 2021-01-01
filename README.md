# CardReader
Gets an image of a card from the webcam input.

This program requires the card to be placed on a solid color piece of paper that contrasts with the pokmone card itself. 
A piece of blue contruction paper worked quite well for me. 

This will take an image, crop it to the colored paper, then look for the card within that, so the background does not need to fill the entire image.
It will correct for perspective, write the card image to a file, and also output the title of the card. 
