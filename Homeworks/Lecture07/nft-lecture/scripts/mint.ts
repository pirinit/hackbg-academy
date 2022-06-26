// We require the Hardhat Runtime Environment explicitly here. This is optional
// but useful for running the script in a standalone fashion through `node <script>`.
//
// When running the script with `npx hardhat run <script>` you'll find the Hardhat
// Runtime Environment's members available in the global scope.
import { utils } from "ethers";
import { ethers } from "hardhat";

async function main() {
  // Hardhat always runs the compile task when running scripts with its command
  // line interface.
  //
  // If this script is run directly using `node` you may want to call compile
  // manually to make sure everything is compiled
  // await hre.run('compile');

  // We get the contract to deploy


  const MyCollection = await ethers.getContractFactory("MyCollection");
  const token = await MyCollection.attach("0xEf33F157Ac43E261aa11ecc33A130d65c65656c4");

  await token.mint(3, {value: utils.parseEther("0.03")});

  await token.reveal();

  console.log("MyContract: 3 NFTs minted:");
}

// We recommend this pattern to be able to use async/await everywhere
// and properly handle errors.
main().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
