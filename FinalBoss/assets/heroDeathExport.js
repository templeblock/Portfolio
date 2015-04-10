//goes in manifest
{src:"heroDeathExport.png", id:"death"}


//goes in load complete
    var deathSheet = new createjs.SpriteSheet({
        images: [queue.getResult("death")],
        frames: [[0,0,150,153,0,37.75,150.25],[0,0,150,153,0,37.75,150.25],[150,0,150,153,0,37.75,150.25],
	[150,0,150,153,0,37.75,150.25],[150,0,150,153,0,37.75,150.25],[150,0,150,153,0,37.75,150.25],
	[300,0,150,153,0,37.75,150.25],[300,0,150,153,0,37.75,150.25],[300,0,150,153,0,37.75,150.25],
	[300,0,150,153,0,37.75,150.25],[300,0,150,153,0,37.75,150.25],[0,153,150,153,0,37.75,150.25],
	[0,153,150,153,0,37.75,150.25],[0,153,150,153,0,37.75,150.25],[0,153,150,153,0,37.75,150.25],
	[0,153,150,153,0,37.75,150.25],[0,153,150,153,0,37.75,150.25],[0,153,150,153,0,37.75,150.25],
	[300,0,150,153,0,37.75,150.25],[300,0,150,153,0,37.75,150.25],[300,0,150,153,0,37.75,150.25],
	[300,0,150,153,0,37.75,150.25],[300,0,150,153,0,37.75,150.25],[150,0,150,153,0,37.75,150.25],
	[150,0,150,153,0,37.75,150.25],[150,0,150,153,0,37.75,150.25],[150,0,150,153,0,37.75,150.25],
	[150,0,150,153,0,37.75,150.25],[0,0,150,153,0,37.75,150.25],[0,0,150,153,0,37.75,150.25]],
        animations: {
            idle: [0, 29, "idle"]
            }     
        });


    //GLOBAL var deathAnimation = new createjs.Sprite(deathSheet);