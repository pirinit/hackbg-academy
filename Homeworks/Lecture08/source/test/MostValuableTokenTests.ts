import { expect } from "chai";
import { ethers } from "hardhat";

import { MostValuableToken } from "../typechain";
import { constants } from "../constants";

describe("MostValuableToken", function () {

    let mvt: MostValuableToken;
    
    beforeEach(async () => {
        const MostValuableToken = await ethers.getContractFactory("MostValuableToken");
        mvt = await MostValuableToken.deploy(constants.MostValuableToken.TOKEN_SUPPLY);
        await mvt.deployed();
    })
  
    it("Should return the proper token name, symbol and total supply", async function () {
      expect(await mvt.totalSupply()).to.equal(constants.MostValuableToken.TOKEN_SUPPLY);
      expect(await mvt.symbol()).to.equal(constants.MostValuableToken.TOKEN_SYMBOL);
      expect(await mvt.name()).to.equal(constants.MostValuableToken.TOKEN_NAME);
    });

});