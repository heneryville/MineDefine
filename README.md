MineDefine
==========

A declarative language for creating Minecraft worlds

For example, the following code will generate a 10 story building.

<pre>
@building: {

  @chair: {
    2x1 @cobblestone;
    up @cobblestone;
  }

  @floor: {
    10x10x1 @stone;
    up;
    4,5 @chair;
    wall 10x10x4 @woodplank;
  }
  @roof: 10x10x1 @stone;

  1x1x10 @floor;
  top @roof;
  top 1x1x3 @wood;
}

@building;
</pre>
